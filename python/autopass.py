#!/usr/bin/python3
import os
import subprocess
import sys
import time

GOPASS = os.getenv("GOPASS_PATH")
if not GOPASS:
    print("Env var GOPASS_PATH must be set")
    sys.exit(1)


def list_passwords():
    """
    List gopass entries
    :return: List of entries
    :rtype: list
    """
    cmd = [GOPASS, 'ls', '--flat']
    return subprocess.check_output(cmd)


def get_fields(entry):
    """
    Get the fields of a gopass entry and returns it as dict
    :entry: A gopass entry
    :ptype: str
    :return: A dict containing the fields of the entry
    :rtype: dict

    Example
    go pass show entry:
    aDummyPasswordXNOCGHZOIDHAO
    url: www.deezer.com
    user: my_name@gmail.com

    return:
    {
        'pass': 'aDummyPasswordXNOCGHZOIDHAO',
        'user': 'my_name@gmail.com',
        'url': 'www.deezer.com,
        'comment': ''
    }
    """
    cmd = [GOPASS, 'show', entry]
    res = {}
    output = subprocess.check_output(cmd)
    for line in output.decode().split('\n'):
        # Check for password line but it can contains ':' char
        if ': ' not in line:
            res['pass'] = line
            continue
        sp = line.split(':')
        res[sp[0]] = sp[1].lstrip()
    return res

def get_entry():
    """
    Get an entry from password list stored in gopass using rofi
    :return: A set containing rofi return code and an entry from the password list
    :rtype: set

    -kb-custom-1 Controle+Return allow will change the return code of rofi
    therefore we can have different behaviors depending on what we type in rofi
    exit code = kb-custom value +10 -1
    e.g: If we type Control+C that is binded to -kb-custom-1 the rofi exit code will be:
    10
    """
    passwords = list_passwords()
    cmd = [
            "rofi", "-sync", "-no-auto-select",
            "-i", "-dmenu",
            "-kb-accept-custom", "", "-kb-custom-1", "Control+Return",
            "-p", "pass: "
          ]
    rofi = subprocess.Popen(cmd, stdout=subprocess.PIPE, stdin=subprocess.PIPE)
    out, _ = rofi.communicate(input=passwords)
    return (rofi.poll(), out.decode().rstrip())


def get_autotype_command(rc, entry, fields):
    """
    Define what autotype to perform.
    An autotype is the list of operation to perform
    such as; [Enter Username, Perform a tab, Entrer password, Perform return]
    :rc: The rofi return code
    :entry: A go pass entry
    :ptype: str
    """
    if rc == 0:
        autotype = ['user', '!Tab', 'pass', '!Return']
        if 'autotype' in fields:
            autotype = fields['autotype'].split(' ')
    elif rc == 10:
        autotype = ['pass', '!Return']
        # to improve:
        if 'ctr_autotype' in fields:
            autotype = fields['autotype'].split(' ')
    return autotype

def do_type():
    """
    Perform the autotype
    """
    rc, entry = get_entry()
    fields = get_fields(entry)
    autotype = get_autotype_command(rc, entry, fields)

    cmds = []
    for val in autotype:
        if val.startswith('!'):
            cmds.append(['xdotool', 'key', val.replace('!', '')])
        else:
            cmds.append(['xdotool', 'type', fields[val]])

    for cmd in cmds:
        time.sleep(0.5)
        subprocess.check_call(cmd)

if __name__ == '__main__':
    do_type()
