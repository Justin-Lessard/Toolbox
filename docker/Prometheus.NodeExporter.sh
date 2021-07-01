How to setup Prometheus Node Exporter on ubuntu

sudo useradd node_exporter -s /sbin/nologin
# TODO Replace with latest version
wget https://github.com/prometheus/node_exporter/releases/download/v1.1.2/node_exporter-1.1.2.linux-amd64.tar.gz

tar xvfz node_exporter-1.1.2.linux-amd64.tar.gz
sudo cp node_exporter-*.*-amd64/node_exporter /usr/sbin/
sudo cp node_exporter-1.1.2.linux-amd64/node_exporter /usr/sbin/

sudo touch /etc/systemd/system/node_exporter.service
sudo vim /etc/systemd/system/node_exporter.service
##############################################################################
[Unit]
Description=Node Exporter

[Service]
User=node_exporter
EnvironmentFile=/etc/sysconfig/node_exporter
ExecStart=/usr/sbin/node_exporter $OPTIONS

[Install]
WantedBy=multi-user.target
##############################################################################
sudo mkdir -p /etc/sysconfig
sudo touch /etc/sysconfig/node_exporter
##############################################################################
#Options go here
##############################################################################
sudo systemctl daemon-reload
sudo systemctl enable node_exporter
sudo systemctl start node_exporter

curl http://localhost:9100/metrics
