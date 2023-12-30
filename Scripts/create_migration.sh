[ $# -lt 1 ] && echo "Usage: $(basename $0) <MIGRATION_NAME>" && exit 1

NAME=$1

cd ../
dotnet ef migrations add $NAME -o Database/Migrations