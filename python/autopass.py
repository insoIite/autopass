#!/usr/bin/env python3
"""Autotype gopass secrets on Wayland (wofi + wtype).

Pick an entry with wofi, then type its fields into the focused window
following the entry's `autotype` sequence, e.g. `user !Tab pass !Return`:
  - a word is the name of a field of the entry (`pass` is the first line)
  - a word starting with `!` is a key name sent as is (`!Tab`, `!Return`...)
"""
import argparse
import os
import shutil
import subprocess
import sys
import time

GOPASS = os.getenv("GOPASS_PATH") or "gopass"

# mode -> (entry field holding the sequence, default sequence)
MODES = {
    "login": ("autotype", "user !Tab pass !Return"),
    "pass": ("pass_autotype", "pass !Return"),
}

# Time for the focus to come back to the target window once wofi is closed
FOCUS_DELAY = 0.3
# Delay between two autotype steps
STEP_DELAY = 0.1


class AutopassError(Exception):
    pass


def list_entries():
    """Return the gopass entries, one per line."""
    return subprocess.run(
        [GOPASS, "ls", "--flat"], capture_output=True, text=True, check=True
    ).stdout


def choose_entry(entries):
    """Let the user pick an entry with wofi. Return None when cancelled."""
    proc = subprocess.run(
        ["wofi", "--dmenu", "--insensitive", "--prompt", "pass"],
        input=entries, capture_output=True, text=True,
    )
    entry = proc.stdout.strip()
    if proc.returncode != 0 or not entry:
        return None
    return entry


def get_fields(entry):
    """Parse a gopass entry into a dict.

    The first line is the password, the following ones are `key: value`:

        aDummyPassword:with:colons
        url: www.deezer.com
        user: my_name@gmail.com

    gives {'pass': 'aDummyPassword:with:colons',
           'url': 'www.deezer.com', 'user': 'my_name@gmail.com'}
    """
    output = subprocess.run(
        [GOPASS, "show", entry], capture_output=True, text=True, check=True
    ).stdout
    password, _, body = output.partition("\n")
    fields = {"pass": password}
    for line in body.splitlines():
        key, sep, value = line.partition(":")
        if sep and key.strip():
            fields[key.strip()] = value.strip()
    return fields


def get_sequence(fields, mode):
    field, default = MODES[mode]
    return fields.get(field, default).split()


def type_text(text):
    # Text goes through stdin so the secret never shows up in `ps`
    subprocess.run(["wtype", "-"], input=text, text=True, check=True)


def press_key(key):
    subprocess.run(["wtype", "-k", key], check=True)


def autotype(fields, sequence):
    # Check everything before typing anything
    missing = [s for s in sequence if not s.startswith("!") and s not in fields]
    if missing:
        raise AutopassError(f"missing field(s): {', '.join(missing)}")

    time.sleep(FOCUS_DELAY)
    for step in sequence:
        if step.startswith("!"):
            press_key(step[1:])
        else:
            type_text(fields[step])
        time.sleep(STEP_DELAY)


def notify(message):
    print(f"autopass: {message}", file=sys.stderr)
    if shutil.which("notify-send"):
        subprocess.run(["notify-send", "autopass", message])


def main():
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument(
        "-m", "--mode", choices=MODES, default="login",
        help="login: use the `autotype` field (default: %s); "
             "pass: use the `pass_autotype` field (default: %s)"
             % (MODES["login"][1], MODES["pass"][1]),
    )
    args = parser.parse_args()

    try:
        entry = choose_entry(list_entries())
        if entry is None:
            return 0
        fields = get_fields(entry)
        autotype(fields, get_sequence(fields, args.mode))
    except FileNotFoundError as e:
        notify(f"command not found: {e.filename}")
        return 1
    except subprocess.CalledProcessError as e:
        notify(f"`{' '.join(e.cmd[:2])}` failed: {(e.stderr or '').strip()}")
        return 1
    except AutopassError as e:
        notify(str(e))
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
