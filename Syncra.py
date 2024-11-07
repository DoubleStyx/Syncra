import os
import subprocess
import sys
from concurrent.futures import ThreadPoolExecutor, as_completed
import platform
import shutil
import traceback
def exception_hook(exctype, value, tb):
    print(f"\nUnhandled Exception:")
    print(f"Type: {exctype.__name__}")
    print(f"Message: {value}")
    print("Traceback (most recent call last):")
    traceback.print_tb(tb)
    sys.exit(1)
sys.excepthook = exception_hook
debug = True
auto_start = True
current_directory = os.path.dirname(os.path.abspath(__file__))
if debug:
    configuration = "Debug"
else:
    configuration = "Release"
def run_command(command, cwd=None):
    try:
        result = subprocess.run(command, cwd=cwd, check=True, capture_output=True, text=True)
        return result.stdout
    except subprocess.CalledProcessError as e:
        print(f"Error occurred while running command: {" ".join(command)}", file=sys.stderr)
        print(f"Standard Output: {e.stdout}", file=sys.stderr)
        print(f"Standard Error: {e.stderr}", file=sys.stderr)
        sys.exit(1)
def build_solution():
    if debug:
        dotnet_build_command = ["dotnet", "build", "--configuration", "Debug"]
    else:
        dotnet_build_command = ["dotnet", "build", "--configuration", "Release"]
    run_command(dotnet_build_command)
def build_workspace():
    if debug:
        cargo_workspace_command = ["cargo", "build"]
    else:
        cargo_workspace_command = ["cargo", "build", "--release"]
    run_command(cargo_workspace_command)
def build_repo():
    with ThreadPoolExecutor() as executor:
        future_to_build = {
            executor.submit(build_solution): "dotnet",
            executor.submit(build_workspace): "cargo"
        }
        for future in as_completed(future_to_build):
            try:
                future.result()
            except Exception as exc:
                print(f"An error occurred: {exc}")
                sys.exit(1)
def copy_repo():
    build_dir = os.path.join(current_directory, "build")
    os.makedirs(build_dir, exist_ok=True)
    syncra_dir = os.path.join(
        current_directory, "Syncra", "bin", f"{configuration.lower()}"
    )
    syncra_rs_dir = os.path.join(
        current_directory, "target", f"{configuration.lower()}"
    )
    try:
        shutil.copytree(syncra_dir, build_dir, dirs_exist_ok=True)
        shutil.copytree(syncra_rs_dir, build_dir, dirs_exist_ok=True)
    except Exception as e:
        sys.exit(1)
def auto_kill():
    if auto_start:
        if platform.system() == "Windows":
            subprocess.run(["taskkill", "/IM", "Syncra.exe", "/F"], shell=True)
        else:
            subprocess.run(["pkill", "Syncra"], shell=True)
def auto_start():
    if auto_start:
        if platform.system() == "Windows":
            subprocess.run(["start", "Syncra.exe"], shell=True)
        else:
            subprocess.run(["./Syncra"], shell=True)
def main():
    auto_kill()
    build_repo()
    copy_repo()
    auto_start()
if __name__ == "__main__":
    main()