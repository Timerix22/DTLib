# Search Tree

byte is dividing into indexes of 3d array of nodes
```
STNode.branches      byte
┃
┣━[0]━┳━[0]━┳━[0]     0
┃     ┃     ┣━...   
┃     ┃     ┗━[3]     3
┃     ┣━...
┃     ┃
┃     ┗━[7]━┳━[0]     28
┃           ┣━...
┃           ┗━[3]     31
┣━...
┃
┗━[7]━┳━[0]━┳━[0]     224
      ┃     ┣━...
      ┃     ┗━[3]     227
      ┣━...
      ┃
      ┗━[7]━┳━[0]     252
            ┣━...
            ┗━[3]     255
```