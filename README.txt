Disclaimer:  Please use it as you see fit, but remember this is very, very alpha.  You should not risk anything other than a small amount of coins (bitcoin or mastercoin) when testing.  To put some context around the risks involved - consider that as we build and broadcast transactions from scratch, if we grab a 100BTC input and miscalculate the change, then a miner somewhere is going to be very happy and you are going to lose out.  I'm encouraging any and all testers to spin up a VM, create a new bitcoin wallet, depsoit a fraction of a bitcoin to cover fees and then use Masterchest software with this new bitcoin wallet.

Requirements: .NET 4, SQL server, bitcoind/qt RPC server with transaction indexing enabled (disabled by default in 0.8+, add txindex=1 in bitcoin.conf and then start with -reindex to readd transaction index).

Initial commit of the Masterchest Engine.

This is a blockchain scanner that will connect to a bitcoind/qt RPC server, scan the blockchain and decode any mastercoin transactions it finds into a database.
It will then run through the database and process the transactions assigning a valid/invalid status to each and calculating the balance for each address accordingly.

This readme will be updated as time allows.  A quick run down of usage is as follows:

When running the engine will connect to an SQL server as specified and scan the blockchain for mastercoin transactions, adding them to the database and then processing them.

An SQL instance is required (can be easily rewritten to use sqlce).  Create a database with the following tables:

balances:
ADDRESS NVARCHAR(100) NOT NULL
CBALANCE BIGINT NOT NULL
CBALANCET BIGINT NOT NULL

processedblocks:
BLOCKNUM INT NOT NULL
BLOCKTIME BIGINT NOT NULL

transactions:
TXID NVARCHAR(100) NOT NULL
FROMADD NVARCHAR(100) NOT NULL
TOADD NVARCHAR(100) NOT NULL
VALUE BIGINT NOT NULL
TYPE NVARCHAR(100) NOT NULL
BLOCKTIME BIGINT NOT NULL
BLOCKNUM INT NOT NULL
VALID BIT NOT NULL
CURTYPE INT NOT NULL
ID INT NOT NULL (identity/auto-increment)

transactions_processed:
TXID NVARCHAR(100) NOT NULL
FROMADD NVARCHAR(100) NOT NULL
TOADD NVARCHAR(100) NOT NULL
VALUE BIGINT NOT NULL
TYPE NVARCHAR(100) NOT NULL
BLOCKTIME BIGINT NOT NULL
BLOCKNUM INT NOT NULL
VALID BIT NOT NULL
CURTYPE INT NOT NULL
ID INT NOT NULL (identity/auto-increment)

Then run masterchest_engine.exe with the following options:

-sqlserv=  'name of sql instance 
-sqldata=  'name of database
-sqluser=  'sql username
-sqlpass=  'sql pass
-bitcoinrpcserv=  'self explanatory
-bitcoinrpcport=
-bitcoinrpcuser= 
-bitcoinrpcpass=

For example you may run it is as a scheduled task every 5 minutes to continually update the database with transactions.
User interfaces for the information can then be built and transactions queried from the database easily, for example:

select * from transactions_processed where txid='txid'
select cbalance from balances where address='address'

