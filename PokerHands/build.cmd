@echo off
pushd src
d:\glassfish4\jdk7\bin\javac -cp .;..\lib\junit-4.11.jar;D:\glassfish4\jdk7\lib; *.java
copy /y *.class ..\com\drewart\Poker
popd
