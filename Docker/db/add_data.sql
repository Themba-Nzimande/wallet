-- LOAD DATAS
INSERT INTO users(id, email, user_password)
VALUES
    (1, 'person1@gmail.com', 'Test1'),
    (2, 'person2@gmail.com', 'Test2'),
    (3, 'person3@gmail.com', 'Test3');


    -- LOAD DATAS
INSERT INTO accounts(id, account_number, current_balance)
VALUES
    (1, 123456789, 100),
    (2, 987654321, 1000),
    (3, 1122334455, 2500);


    -- LOAD DATAS
INSERT INTO user_accounts(id, user_id, account_id)
VALUES
    (1, 1, 1),
    (2, 2, 2),
    (3, 3, 3);