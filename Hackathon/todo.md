Services: non singletons handling outside things.
Modules: Command/features for input/output layer
Managers: actual functionality called by modules.


ShopModule/Manager:
- Difference between showShop and showItem.
	- showShop is browsing entire list, with no option to buy and small description. 
	- showItem does single item at a time nav list, with large description and buy option.
		- showItem is a list, because it can support search terms, however, generally its for 1 item.

Reserved words:
- INVENTORY
- 'all items'


playerviewing inventory MUST be emphreal?

# TODO

- use embeds whenever possible
- make private info ehpemeral (only shows to command sender)
- Permissions are command group based, so admin commands need to be in seperate group

## Overall
1. toString the main objects, for outputting to discord
2. maybe also custom tostring with specifc controlled info to feed ai

## Admin
1. add and remove things.
	1. No remove. add command for json, so I can add without needing to open mongoCompass, however, this is a common case so no point on implementing these features.
	1. However, being able to edit SPECIFIC player info will be benificial (name, items, background, etc).
	1. The point being, adding functionality to add/remove json blocks is pointless.

## SHOP
1. ~~buy~~
2. ~~sell~~
3. add more stuff
1. ~~Modify price and buy and sell~~
1. Use itemId instead of names. USE ITEM ID, using name does EVERYTHING sharing the name
1. Fix shop nav after selling item

## PLAYER
1. Advance display
	1. ~~Inventory~~
	1. lore
	1. stats
2. edit player
3. add player?!?!

## AI
1. Everything
1. conversations