FROM confluentinc/cp-kafka-connect-base:7.6.0

RUN confluent-hub install --no-prompt confluentinc/kafka-connect-jdbc:10.7.4
COPY worker.properties .
COPY tfa-source-connector.properties .

CMD ["/bin/connect-standalone", "worker.properties", "tfa-source-connector.properties"]