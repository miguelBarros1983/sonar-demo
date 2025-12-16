pipeline {
    agent any

    environment {
        SONAR_TOKEN = credentials('SONARQUBE_TOKEN')
        DOTNET_TOOLS = "/var/jenkins_home/.dotnet/tools"
    }

    stages {

        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/miguelBarros1983/sonar-demo.git'
            }
        }

        stage('Prepare SonarQube') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        export PATH="\$PATH:$DOTNET_TOOLS"
                        dotnet-sonarscanner begin \
                            /k:"demo-app" \
                            /d:sonar.host.url="http://sonarqube:9000" \
                            /d:sonar.token="$SONAR_TOKEN" \
                            /d:sonar.exclusions="**/.vs/**,**/bin/**,**/obj/**"
                    """
                }
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

        stage('SonarQube End') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        export PATH="\$PATH:$DOTNET_TOOLS"
                        dotnet-sonarscanner end /d:sonar.token="$SONAR_TOKEN"
                    """
                }
            }
        }

        stage('Quality Gate') {
            steps {
                timeout(time: 10, unit: 'MINUTES') {
                    // Delay necessário para o CE terminar e a API atualizar o estado
                    sleep(time: 5, unit: 'SECONDS')
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}
