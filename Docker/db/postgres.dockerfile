FROM postgres:15.1-alpine

LABEL author="tester"
LABEL description="assesment Postgres DB"
LABEL version="1.0"

COPY *.sql /docker-entrypoint-initdb.d/

# ADD ./scripts/init.sql /docker-entrypoint-initdb.d/
ADD ./sql/create_tables.sql:/docker-entrypoint-initdb.d/create_tables.sql
ADD ./sql/add_data.sql:/docker-entrypoint-initdb.d/fill_tables.sql
RUN chown postgres:postgres /docker-entrypoint-initdb.d/init.sql