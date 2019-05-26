#Autopass

Autopass is a tool that allow you to automatically type your secrets.
Secrets can be user/password of a websites, a ssh password, etc ...

## Linux (debian/ubuntu)

### Requirements
  * gopass is installed and GOPASS_PATH env var is set
  * [rofi](https://github.com/davatorium/rofi) is installed
  * python3 is installed

##Windows
### Requirements
  * Windows 10
  * gopass is installed and the exe path is set to $PATH
  * The application is installed in the StartMenu
  * The application as shortut set
  * Gopass entries have a autotype key value set such as:
  ```
  user: foo # or foo.bar@mail.com
  autotype: user !Tab pass !Return
  ```
  
### Usage
  * With the above autotype, the program will:
    * type user
	* perform a Tab
	* type password
	* perform a Return (ENTER touch)
  * Perform shortcut
  * Search your entry
  * Type enter

### Issues
#### Special chars
If the password a special chars such as '+', '%', '^' then they will be interprated by SendKeys instead of just typing it.\\
The program will enclose those chars with '{}' as asked by the SendKeys documentation.\\
see [SenKeys API](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=netframework-4.8) is installed

#### Keyboard Layout
The program use the Windows SendKeys API that doesn't work well with keyboard layout different than EN_US.\\
Therefore it will change the keyboard layout before typing the entry and will switch back to default afterward