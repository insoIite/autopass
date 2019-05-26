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

## Linux (debian/ubuntu)
### Requirements
  * gopass is installed and GOPASS_PATH env var is set
  * [rofi](https://github.com/davatorium/rofi) is installed
  * python3 is installed

### Usage
  * Launch script with dmenu or from the consle
  * Search your entry
  * Type enter

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

