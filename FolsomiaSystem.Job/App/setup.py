# -*- coding: utf-8 -*-
import sys
from cx_Freeze import setup, Executable

# Dependencies are automatically detected, but it might need fine tuning.
build_exe_options = {"packages": ['os',
            'cv2',
            'numpy',
            'argparse',
            'logging',

], "excludes": ["tkinter"]}

# GUI applications require a different base on Windows (the default is for a
# console application).
base = None
if sys.platform == "win32":
    base = "Console"

setup(  name = "folsomiaCount",
        version = "0.1",
        description = "Folsomia Count application!",
        options = {"build_exe": build_exe_options,
                'bdist_msi': {
                'add_to_path': False,
                'environment_variables': [
                    ("FOLSOMIA_COUNT", "=-*FOLSOMIA_COUNT", r"[~];[TARGETDIR]", "TARGETDIR")
                ]
            }
        },
        executables = [Executable("folsomiacount.py", base=base)]
        
        
        
        )