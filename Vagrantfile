Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"
  config.vm.network "private_network", ip: "192.168.99.100"
  config.vm.provider "virtualbox" do |vb|
    vb.memory = "6144"
    vb.cpus = 4
  end
  config.vm.provision "docker"
  config.vm.provision "shell", inline: <<-SHELL
    # docker-compose
    curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose

    cd /vagrant
    sudo docker-compose up
  SHELL
  config.vm.network "forwarded_port", guest: 5031, host: 5031
  config.vm.network "forwarded_port", guest: 5030, host: 5030
  config.vm.network "forwarded_port", guest: 3000, host: 3000
  config.vm.network "forwarded_port", guest: 9090, host: 9090
end