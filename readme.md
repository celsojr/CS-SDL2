# Installing SDL2 and CSharp bindings

## Search all sdl2 libs
### Linux
```sh
apt-cache search libsdl2
```
### macOS
```sh
brew search SDL2
```
## Install the desired SDL libs
### Linux
```sh
apt install libsdl2-2.0-0
apt install libsdl2-image-2.0-0
```
### macOS
```sh
brew install SDL2
brew install SDL2_image

# check
ls -l /opt/homebrew/lib/libSDL2.dylib

# create a symbolic if needed
echo 'export DYLD_LIBRARY_PATH=/opt/homebrew/lib:$DYLD_LIBRARY_PATH' >> ~/.zshrc
```
## Download the c# files with the wrapper methods for SDL2
### wget
```sh
mkdir SDL
cd SDL
wget https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2.cs
wget https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2_image.cs
```
### curl
```sh
mkdir SDL
cd SDL
curl -LO https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2.cs
curl -LO https://github.com/flibitijibibo/SDL2-CS/raw/master/src/SDL2_image.cs
```
