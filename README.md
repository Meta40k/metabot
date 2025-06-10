sudo add-apt-repository ppa:dotnet/backports
sudo apt update 
sudo apt-get install -y dotnet-sdk-9.0
sudo apt-get install -y aspnetcore-runtime-9.0
sudo apt-get install -y dotnet-runtime-9.0
dotnet --version


export TELEGRAM_BOT_TOKEN_SIRI=""
export CONN_STRING=""

echo $TELEGRAM_BOT_TOKEN_SIRI

для тестирования в RIDER добавить перменные в Run → Edit Configurations...
айди поле "Environment variables"

sudo apt install postgresql postgresql-contrib -y

pg_config --version
PostgreSQL 16.9 (Ubuntu 16.9-0ubuntu0.24.04.1)

sudo systemctl status postgresql

sudo -i -u postgres
psql

CREATE DATABASE metabot;
CREATE USER metabot_user WITH ENCRYPTED PASSWORD 'strongpassword!';
GRANT ALL PRIVILEGES ON DATABASE metabot TO metabot_user;


psql "Host=localhost;Port=5432;Database=metabot;Username=metabot_user;Password=STRONGPASS"



CREATE SCHEMA IF NOT EXISTS bot_data;


CREATE TABLE bot_data.users (
user_id BIGINT PRIMARY KEY,
username TEXT,
first_name TEXT,
last_name TEXT,
created_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);


CREATE TABLE bot_data.messages (
id SERIAL PRIMARY KEY,
chat_id BIGINT NOT NULL,
user_id BIGINT NOT NULL REFERENCES bot_data.users(user_id),
message_type TEXT NOT NULL DEFAULT 'text',
message_text TEXT,
file_id TEXT,
file_size BIGINT,
mime_type TEXT,
sent_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);



