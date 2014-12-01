@echo off

svn log -v .. --xml > logfile.log

rem ### UTF-8
rem ### java -jar statsvn.jar -output-dir Output -title "Brain of teh Zwarm" -charset UTF-8 -disable-twitter-button -exclude "Library/**;Assets/UVersionControl/**;Statistics/**;Dokumentation/**;**/*.meta" logfile.log ..

rem ### default charset

java -jar statsvn.jar -output-dir Output -title "Deathstar PD" -disable-twitter-button -exclude "DeathstarPD/Library/**;DeathstarPD/Assets/UVersionControl/**;Statistics/**;Dokumentation/**;UVG Editor/**;**/*.meta;**/*.csproj;**/*.sln;**/*.userprefs;**/*.prefab;**/*.asset;**/*.mat;**/*.uvg;**/*.svg" logfile.log ..