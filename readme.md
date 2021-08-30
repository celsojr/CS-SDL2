# Installing SDL2 and CSharp bindings on Ubuntu

## Search all sdl2 libs
```sh
apt-cache search libsdl2
```
## Install the desired libs
```sh
apt install libsdl2-2.0-0
apt install libsdl2-image-2.0-0
apt install libsdl2-ttf-2.0-0
```
## Download the c# files with the wrapper methods for SDL2
```sh
mkdir SDL
cd SDL
wget https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2.cs
wget https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2_image.cs
```
