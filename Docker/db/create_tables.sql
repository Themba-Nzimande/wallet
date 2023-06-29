-- CREATE TYPE
-- CREATE TABLE
DROP TABLE IF EXISTS users;
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    email VARCHAR NOT NULL,
    user_password VARCHAR NOT NULL
);

-- CREATE TABLE
DROP TABLE IF EXISTS user_accounts;
CREATE TABLE user_accounts (
    id SERIAL PRIMARY KEY,
    user_id SERIAL,
    account_id SERIAL
);

-- CREATE TABLE
DROP TABLE IF EXISTS accounts;
CREATE TABLE accounts (
    id SERIAL PRIMARY KEY,
    account_number bigint,
    current_balance real
);

-- CREATE TABLE
DROP TABLE IF EXISTS accounts_transactions;
CREATE TABLE accounts_transactions (
    id SERIAL PRIMARY KEY,
    amount real,
    transaction_timestamp timestamp without time zone,
    account_id SERIAL,
    transcation_entry_type VARCHAR NOT NULL
);