-- create a table
do
$$
BEGIN
    IF EXISTS (SELECT * FROM pg_user WHERE usename = 'palisaid') THEN  
        DROP ROLE palisaid; 
    END IF;
 
    CREATE ROLE palisaid WITH LOGIN INHERIT CREATEDB CREATEROLE REPLICATION PASSWORD 'password';
END
$$
;
