import subprocess

def clearTemp():
    clearUserTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    clearSysTemp = subprocess.run(
        ["powershell", "-Command", "Remove-Item -Path \"$env:windir\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    clearEmpy = subprocess.run(
        ["powershell","-Command","Clear-RecycleBin -Force -ErrorAction SilentlyContinue"],
        capture_output=True, text=True
    )

    erros = []
    if clearUserTemp.stderr.strip():
        erros.append("Temp do Usuário")
    if clearSysTemp.stderr.strip():
        erros.append("Temp do Sistema")
    if clearEmpy.stderr.strip():
        erros.append("Lixeira")

    if erros:
        return f"Ocorreu um erro ao limpar: {', '.join(erros)}"
    else:
        return "Arquivos temporários limpos com sucesso"

print(clearTemp())
