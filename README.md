# Autopass

Autopass is a tool that allow you to automatically type your secrets.
Secrets can be user/password of a websites, a ssh password, etc ...

## Gopass entries
The program will type your login/password or just whatever you want depending on the 'autotype' key of the password entry.

Let's say you have an entry looking like these:
```
gopass edit website/github
output:

ARdmPassword
user: foo.bar@mail.com
autotype: user !Tab pass !Return
```
Then the program will:
  * Type the 'user' key value
  * Type the Tab key
  * Type the 'pass' key value (previously set by the application)
  * Type the Enter/Return key

## Linux (Wayland, wlroots compositors such as Sway)
### Requirements
  * python3
  * gopass, in `$PATH` or pointed to by the `GOPASS_PATH` env var
  * [wofi](https://hg.sr.ht/~scoopta/wofi) to pick the entry
  * [wtype](https://github.com/atx/wtype) to type it (`sudo apt install wtype`)
  * optionally `notify-send` to get error notifications

### Usage
  * `autopass.py` types the `autotype` sequence of the entry (default: `user !Tab pass !Return`)
  * `autopass.py --mode pass` types the `pass_autotype` sequence (default: `pass !Return`)

Example Sway bindings:
```
bindsym $mod+p exec ~/projects/perso/autopass/python/autopass.py
bindsym $mod+Shift+p exec ~/projects/perso/autopass/python/autopass.py --mode pass
```

##Windows
### Requirements
  * Windows 10
  * en_us keyboard layout is installed on the windows
  * [Gopass](https://github.com/gopasspw/gopass/releases) is installed and the exe path is set to $PATH
  * The application is installed in the StartMenu
  * The application as a shortut set (Shortcut can only be configured on the startMenu)
  * [Gopass](https://github.com/gopasspw/gopass/releases) entries have key/value set such as:
  ```
  user: foo.bar@mail.com
  autotype: user !Tab pass !Return
  ```
  
### Usage
  * Perform shortcut
  * Search your entry
  * Type enter

### Issues
#### Keyboard Layout
The application use the Windows SendKeys API that doesn't work well with keyboard layout different than EN_US.  
Therefore it will change the keyboard layout before typing the entry and will switch back to default afterward

#### Special chars
If the password a special chars such as '+', '%', '^' then they will be interprated by SendKeys instead of just typing them.  
The application will enclose those chars with '{}' as asked by the SendKeys documentation.\\
see [SenKeys API](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=netframework-4.8) is installed

