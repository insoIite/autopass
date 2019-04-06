#!/usr/bin/python3
import subprocess

GOPASS = '/usr/local/bin/gopass'

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
    comment:
    url: fdu.com
    username: dugast.fabien@gmail.com

    return:
    {
        'pass': 'aDummyPasswordXNOCGHZOIDHAO',
        'user': 'dugast.fabien@gmail.com',
        'url': 'fdu.com,
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
    :return: An entry from the password list
    :rtype: str
    """
    passwords = list_passwords()
    cmd = ["rofi", "-sync", "-no-auto-select", "-i", "-dmenu", "-p", "pass: "]
    rofi = subprocess.Popen(cmd, stdout=subprocess.PIPE, stdin=subprocess.PIPE)
    out, _ = rofi.communicate(input=passwords)
    return out.decode().rstrip()


def get_autotype_command(entry):
    """
    Define what autotype to perform.
    An autotype is the list of operation to perform
    such as; [Enter Username, Perform a tab, Entrer password, Perform return]
    :entry: A go pass entry
    :ptype: str
    """
    autotype = ['user', '!Tab', 'pass']
    fields = get_fields(entry)

    if 'ssh' in entry:
        autotype = ['pass', 'Return']
    elif 'website' in entry and 'autotype' in fields:
        if 'autotype' in fields:
            autotype = [value for value in fields['autotype'].split(' ')]
    else:
        if 'autotype' in fields:
            autotype = [fields[value] for value in fields]
    return autotype

def do_type():
    """
    Perform the autotype
    """
    entry = get_entry()
    fields = get_fields(entry)
    autotype = get_autotype_command(entry, fields)

    cmds = []
    for val in autotype:
        if val.startswith('!'):
            cmds.append(['xdotool', 'key', val.replace('!', '')])
        else:
            cmds.append(['xdotool', 'type', fields[val]])

    if 'confirm' not in fields or fields['confirm']:
        cmds.append(['xdotool', 'key', 'Return'])

    for cmd in cmds:
        subprocess.check_call(cmd)

if __name__ == '__main__':
    do_type()
