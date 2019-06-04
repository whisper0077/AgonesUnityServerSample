start-minikube:
	minikube profile agones
	minikube start --kubernetes-version v1.11.0 --vm-driver=hyperv --extra-config=apiserver.authorization-mode=RBAC --hyperv-virtual-switch="minikube" --v=3

install-agones:
	kubectl create namespace agones-system
	kubectl create clusterrolebinding cluster-admin-binding --clusterrole=cluster-admin --serviceaccount=kube-system:default
	kubectl apply -f https://raw.githubusercontent.com/GoogleCloudPlatform/agones/release-0.10.0/install/yaml/install.yaml

build-image:
	docker build -t agones-unity-server-sample:1.0 .

