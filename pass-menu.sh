#!/usr/bin/env bash
shopt -s nullglob globstar


# config
clip=primary
rofi="rofi -sync -no-auto-select -i -dmenu"
autotype_field=autotype
delay=2
autoenter=false


cachedir=${XDG_CACHE_HOME:-$HOME/.cache}
if [ -d "$cachedir" ]; then
	historyfile=$cachedir/pass_history
else			# if no xdg dir, fall back to dotfiles in ~
	historyfile=$HOME/.pass_history
fi

target_window=$(xdotool getwindowfocus)


list_passwords() {
	passwords=$(gopass ls --flat)

	printf '%s\n' "$passwords" | awk -v histfile=$historyfile '
		BEGIN {
			while( (getline < histfile) > 0 ) {
				sub("^[0-9]+\t","")
				print
				x[$0]=1
			}
		} !x[$0]++'
}

password_selected() {
	echo "$1" | awk -v histfile=$historyfile '
		BEGIN {
				FS=OFS="\t"
				while ( (getline < histfile) > 0 ) {
					count=$1
					sub("^[0-9]+\t","")
					fname=$0
					history[fname]=count
				}
				close(histfile)
			}

			{
				history[$0]++
			}

			END {
				if(!NR) exit
				for (f in history)
					print history[f],f | "sort -t '\t' -k1rn >" histfile
			}
		'
}

do_clip() {
	case "$clip" in
		"primary") xclip;;
		"clipboard") xclip -selection clipboard;;
		"both") xclip; xclip -o | xclip -selection clipboard;;
	esac

	notify-send "pass: field copied to primary clipboard"
}

do_type() {
	x_repeat_enabled=$(xset q | awk '/auto repeat:/ {print $3}')
	xset r off

	xdotool type --window $target_window --clearmodifiers --file -

	xset r "$x_repeat_enabled"
	unset x_repeat_enabled

	notify-send "pass: field typed"
}

do_autotype() {
	x_repeat_enabled=$(xset q | awk '/auto repeat:/ {print $3}')
	xset r off

	autotype=$(get_field $1 $autotype_field)
	if [ -z "$autotype" ]; then
		autotype="user :tab pass"
	fi

	for word in $autotype; do
		case "$word" in
			":tab") xdotool key --window $target_window Tab;;
			":space") xdotool key --window $target_window space;;
			":delay") sleep "${delay}";;
			":enter") xdotool key --window $target_window Return;;
			*) get_field $1 "$word" | xdotool type --window $target_window --clearmodifiers --file -;;
		esac
	done

	if [[ ${auto_enter} == "true" ]]; then
		xdotool key --window $target_window Return
	fi

	xset r "$x_repeat_enabled"
	unset x_repeat_enabled

	notify-send "pass: autotype completed"
}

list_fields() {
	echo password
	gopass show "$1" | grep -io "^.*: " | sed 's/:.*$//ig'
}

get_field() {
	case "$2" in
		password|pass)
			gopass show -f "$1" | head -n 1 | head -c -1
			;;
		*)
			gopass show "$1" | grep -i "^$2: " | sed "s/^$2: //ig" | head -c -1
			;;
	esac
}

select_field() {
	fields=$(list_fields $1)

	field=$(echo "$fields" | $rofi -kb-accept-custom "" -kb-accept-alt "" -kb-custom-1 Control+c \
		-mesg "Select field for $1" -p "pass:")

	rofi_exit=$?
	if [[ $rofi_exit -eq 1 ]]; then
		exit
	fi

	# Actions based on exit code, which do not need the entry.
	# The exit code for -kb-custom-X is X+9.
	case "${rofi_exit}" in
		0) password_selected $1; get_field $1 $field | do_type; return;;
		10) password_selected $1; get_field $1 $field | do_clip; return;;
	esac

	exit 1
}

select_password() {
	select=$(list_passwords | $rofi -kb-accept-custom "" -kb-accept-alt "" -kb-custom-1 Control+c \
		-kb-custom-2 Shift+Return -kb-custom-3 Control+Return -p "pass:")

	rofi_exit=$?
	if [[ $rofi_exit -eq 1 ]]; then
		exit
	fi

	# Actions based on exit code, which do not need the entry.
	# The exit code for -kb-custom-X is X+9.
	case "${rofi_exit}" in
		0)  password_selected $select; get_field $select password | do_type; return;;
		10) password_selected $select; get_field $select password | do_clip; return;;
		11) select_field $select; return;;
		12) password_selected $select; do_autotype $select; return;;
	esac

	exit 1
}


select_password
