import subprocess

clearTEMP = subprocess.run(
    ["powershell", "-Command", "Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue"]    
)

if clearTEMP.returncode == 0:
    print("Arquivos Temporarios Limpos com sucesso")
else:
    print("Ocorreu um erro ao limpar os Arquivos Temporarios")


