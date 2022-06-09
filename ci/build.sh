set -e
set -x

dotnet publish CoBets/TelegramBot/TelegramBot.csproj -c Release

cp -r CoBets/TelegramBot/bin/Release/net5.0/publish $1
cp .dockerignore $1/.dockerignore
