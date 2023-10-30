Vagrant.configure("2") do |config|
  config.vm.box = "radetich/ubuntu-rosetta"
  config.vm.box_version = "3.0"

  config.vm.network "forwarded_port", guest: 44329, host: 1234
  config.vm.provider "virtualbox" do |vb|
     vb.gui = false
     vb.memory = "1024"
  end

  config.vm.provision "shell", inline: <<-SHELL
      # Оновлюємо систему
      sudo apt-get update -q
      sudo apt-get upgrade -y -q

      # Вирішуємо проблеми зі зламаними залежностями
      sudo apt-get install -f -y -q

      # Встановлюємо необхідні пакети
      sudo apt-get install -y -q software-properties-common apt-transport-https curl

      # Додаємо репозиторій Universe
      sudo add-apt-repository -y universe

      # Додаємо ключ Microsoft репозиторію
      curl -sSL https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -

      # Додаємо Microsoft репозиторій
      sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-$(lsb_release -cs)-prod $(lsb_release -cs) main" > /etc/apt/sources.list.d/microsoft.list'
      
      sudo apt-get update -q

      # Встановлюємо .NET SDK 7.0
      sudo apt-get install -y -q dotnet-sdk-7.0

      # Додаємо шлях до .NET tools
      echo 'export PATH=$PATH:$HOME/.dotnet/tools' >> /home/vagrant/.bashrc

      # Додаємо NuGet джерело і встановлюємо .NET tool
      su - vagrant -c "wget -qO- https://aka.ms/install-artifacts-credprovider.sh | bash"
      su - vagrant -c "dotnet nuget add source https://pkgs.dev.azure.com/Pfriedrix/_packaging/Pfriedrix/nuget/v3/index.json --name Pfriedrix --username Ivan --password cros2hsu4wljdpt3gezb6dwx44nwgefsvum27222bifzqhotnt5q --store-password-in-clear-text"
      su - vagrant -c "dotnet tool install --global CrossFitLab4"

      # Очищуємо кеш
      sudo apt-get clean
      sudo apt-get autoclean
  SHELL
end