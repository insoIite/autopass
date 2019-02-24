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
    """ Get the fields from a gopass entry """
    cmd = [GOPASS, 'show', entry]
    res = {}
    output = subprocess.check_output(cmd)
    for line in output.decode().split('\n'):
        if ':' not in line:
            res['password'] = line
            continue
        sp = line.split(':')
        res[sp[0]] = sp[1].lstrip()
    return res

def get_entry():
    passwords = list_passwords()
    cmd = ["rofi", "-sync", "-no-auto-select", "-i", "-dmenu", "-p", "pass: "]
    rofi = subprocess.Popen(cmd, stdout=subprocess.PIPE, stdin=subprocess.PIPE)
    out, _ = rofi.communicate(input=passwords)
    return out.decode().rstrip()

if __name__ == '__main__':
    entry = get_entry()

    res = get_fields(entry)
    print(res)
