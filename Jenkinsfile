pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('SONARQUBE_TOKEN')
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/miguelBarros1983/sonar-demo.git'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore MinhaAppCli/MinhaAppCli.csproj'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build MinhaAppCli/MinhaAppCli.csproj --no-restore'
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        sonar-scanner \
                        -Dsonar.projectKey=demo-app \
                        -Dsonar.sources=MinhaAppCli \
                        -Dsonar.host.url=http://sonarqube:9000 \
                        -Dsonar.login=$SONAR_TOKEN
                    """
                }
            }
        }

        stage('Quality Gate') {
            steps {
                timeout(time: 2, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}
