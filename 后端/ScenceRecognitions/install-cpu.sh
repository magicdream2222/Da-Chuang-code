#!/bin/bash
#create conda envirmonent
conda create -n project_test python=3.6
source activate project
pip install easydict -i https://pypi.tuna.tsinghua.edu.cn/simple/ ##select china source
pip install Cython opencv-python -i https://pypi.tuna.tsinghua.edu.cn/simple/
pip install matplotlib -i https://pypi.tuna.tsinghua.edu.cn/simple/
pip install -U pillow -i https://pypi.tuna.tsinghua.edu.cn/simple/
pip install  h5py lmdb mahotas -i https://pypi.tuna.tsinghua.edu.cn/simple/
pip install tensorflow==1.4
pip install http://download.pytorch.org/whl/cpu/torch-0.3.1-cp36-cp36m-linux_x86_64.whl 
pip install torchvision
sudo apt-get install python-numpy
cd ./ctpn/lib/utils
chmod +x make.sh
sh ./make.sh

