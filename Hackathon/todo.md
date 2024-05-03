Services: non singletons handling outside things.
Modules: Command/features for input/output layer
Managers: actual functionality called by modules.


ShopModule/Manager:
- Difference between showShop and showItem.
	- showShop is browsing entire list, with no option to buy and small description. 
	- showItem does single item at a time nav list, with large description and buy option.
		- showItem is a list, because it can support search terms, however, generally its for 1 item.

# TODO

- use embeds whenever possible
- make private info ehpemeral (only shows to command sender)
- Permissions are command group based, so admin commands need to be in seperate group

## Overall
1. toString the main objects, for outputting to discord
2. maybe also custom tostring with specifc controlled info to feed ai

## SHOP
1. buy
2. sell
3. add more stuff

## PLAYER
1. proper display
2. edit player
3. add player?!?!

## AI
1. Everything