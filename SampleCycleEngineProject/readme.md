how scripts works?

Engine -> init script (script.init) -> chapter script (script.onload)
one script should only contains one chapter

access modifier for variable?
global -> shares among whole game (across different saves)
local -> only available locally (save that declare the variable)

NOTICE: global variable declaration is only avaliable in init script. 
Using global access in chapter ces will throw compile error

scope of accessing entities?
for commands in ces, they can only access entities thats declared before the command that is in the same command block section, and they can access entities declared in the onload section of the script or the init section in the init script.
you cannot access entities declared in other script's onload section.