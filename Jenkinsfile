pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('SONARQUBE_TOKEN')
    }

    stages {

        stage('Checkout Source Code') {
            steps {
                retry(3) {
                    git branch: 'main', url: 'https://github.com/miguelBarros1983/sonar-demo.git'
                }
            }
        }

        stage('SonarQube Begin') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        dotnet sonarscanner begin \
                            /k:"demo-app" \
                            /d:sonar.host.url="http://sonarqube:9000" \
                            /d:sonar.token="$SONAR_TOKEN"
                    """
                }
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore MinhaAppCli/MinhaAppCli.csproj'
            }
        }

        stage('Compile Application') {
            steps {
                sh 'dotnet build MinhaAppCli/MinhaAppCli.csproj --no-restore'
            }
        }

        stage('SonarQube Finalize') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
                    """
                }
            }
        }

        stage('Quality Gate Validation') {
            steps {
                timeout(time: 10, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }

    post {
        success {
            echo "✅ Pipeline concluído com sucesso"
        }
        failure {
            echo "❌ Pipeline falhou"
        }
    }
}
