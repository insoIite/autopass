Goal
----

As a developer I want to auto enter password from keepassx database.

AC
--

  * Password are still manually added to keepassx
  * Program automatically take in account new password added
  * Works with multiple keepass databases
  * We can auto enter password for shell, ssh, gpg, web formular (login, etc...)

Design
------
As keepass has no API or command cli in order to do it, we will use `gopass` to store our passwords

  * gopass have to synchronise from many keepass database
  * gopass architecture is done correctly (name / folder /entries)
  * We can find a password entry by search feature with a dmenu, rofi or other
  * Enter password into stdin
