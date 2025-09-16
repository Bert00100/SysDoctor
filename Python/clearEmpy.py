import subprocess

def clearEmpy():
    clearEmpy = subprocess.run(
        ["powershell", "-Command", "Clear-RecycleBin -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    if clearEmpy.returncode != 0:
        return "Lixeira limpa com sucesso"
    else:
        return f"Erro ao limpar a lixeira: {clearEmpy.stderr.strip()}"

print(clearEmpy())

     
