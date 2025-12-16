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

        stage('Prepare SonarQube') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        dotnet tool install --global dotnet-sonarscanner || true
                        export PATH="\$PATH:/root/.dotnet/tools"
                        dotnet sonarscanner begin \
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
                        export PATH="\$PATH:/root/.dotnet/tools"
                        dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
                    """
                }
            }
        }

        stage('Quality Gate') {
            steps {
                timeout(time: 10, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}
