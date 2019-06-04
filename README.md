# AgonesUnityServerSample
Unity Server Sample for [Agones](https://github.com/GoogleCloudPlatform/agones)

## Install and Prerequisites
This sample is tested on
```
Unity : Unity 2018.4.1f1
Platform : Windows 10 Pro
Docker : Version 2.0.0.3 (31259)
Minikube: v1.1.0
```
and Using Powershell

* Docker for Windows
  * https://hub.docker.com/editions/community/docker-ce-desktop-windows
* Minikube
  * https://kubernetes.io/docs/tasks/tools/install-minikube/

## Running on UnityEditor
* Open `AgonesUnityServerSample` with UnityEditor.
* Select `AgonesEchoSample` scene.
* Play!
  * A Echo Client and a Local Echo Server will run.

## Running a Server on Agones
There is a few steps.

### Setting a Minikube Cluster and Install Agones
* Please see [Agones Document](https://agones.dev/site/docs/installation/)
  * Setting up a Minikube cluster
  * Enabling creation of RBAC resources
  * Installing Agones

  Or you can use `make` command instead
    ```
    make start-agones
    make install-agones
    ```

### Building a Server
* Hit a `Build Tool/Build Server` menu item in the menu bar.
  * The Builds are created in a `Builds/Server` Folder.

### Building a Docker Container Image
  ```
  docker build -t agones-unity-server-sample:1.0 .
  ```

### Running a Unity Server Container on Agones
* Use local docker images on Minikube
  ```
   & minikube docker-env | Invoke-Expression
  ```
* Now, you can run a Unity Server on Agones with the following command!
  ```
  kubectl create -f gameserver.yaml
  ```


Thanks!!!