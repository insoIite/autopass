Requirements
------------
  * gopass is installed

Installing gopass (debian/ubuntu)
-----------------
From [https://github.com/gopasspw/gopass/blob/master/docs/setup.md#ubuntu--debian-1]

```bash
wget -q -O- https://api.bintray.com/orgs/gopasspw/keys/gpg/public.key | sudo apt-key add -
echo "deb https://dl.bintray.com/gopasspw/gopass trusty main" | sudo tee /etc/apt/sources.list.d/gopass.list

sudo apt-get update
sudo apt-get install gopass
```

