Disclaimer:  Please use it as you see fit, but remember this is very, very alpha.  You should not risk anything other than a small amount of coins (bitcoin or mastercoin) when testing.  To put some context around the risks involved - consider that as we build and broadcast transactions from scratch, if we grab a 100BTC input and miscalculate the change, then a miner somewhere is going to be very happy and you are going to lose out.  I'm encouraging any and all testers to spin up a VM, create a new bitcoin wallet, depsoit a fraction of a bitcoin to cover fees and then use Masterchest software with this new bitcoin wallet.

Requirements: .NET 4, bitcoind/qt RPC server with transaction indexing enabled (disabled by default in 0.8+, add txindex=1 in bitcoin.conf and then start with -reindex to readd transaction index).

Initial commit of the Masterchest Wallet.

Please see bitcointalkurl for details.
