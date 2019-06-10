# AgonesUnityServerSample
A Dedicated Server Sample built in Unity, It uses [Agones](https://github.com/GoogleCloudPlatform/agones) Rest SDK.

## Install and Prerequisites
This sample is working on
```
Unity Editor: Unity 2018.4.1f1
OS: Windows 10 Pro
Docker: Version 2.0.0.3 (31259)
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
  * An Echo Client and a Local Echo Server will run.

## Running a Dedicated Server on Agones
There are a few steps.

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

### Building a Dedicated Server
* Open `AgonesUnityServerSample` with UnityEditor.
* Hit a `Build Tool/Build Server` menu item in the menu bar.
  * The Builds are created in a `Builds/Server` Folder.

### Building a Docker Container Image for Agones
  ```
  docker build -t agones-unity-server-sample:1.0 .
  ```

### Running a Dedicated Server Container on Agones
* Use local docker images on Minikube
  ```
   & minikube docker-env | Invoke-Expression
  ```
* Now, you can run a Unity Server on Agones with the following command!
  ```
  kubectl create -f gameserver.yaml
  ```


Thanks!!!