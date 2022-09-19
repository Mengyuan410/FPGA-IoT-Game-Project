import matplotlib.pyplot as plt
import numpy as np
from matplotlib.collections import LineCollection
from matplotlib.lines import Line2D

alpharaw = []
thetaraw = []
zraw = []
alphaf = []
thetaf = []
zf = []
speed = []

file = open('data.txt', 'r')
count = 0
while True:
    count += 1
    line = file.readline()
    if not line:
        break
    else:
        data = line.split(',')     
        alpharaw.append(float(data[0]))
        thetaraw.append(float(data[1]))
        zraw.append(float(data[2]))
        alphaf.append(float(data[3]))
        thetaf.append(float(data[4]))
        zf.append(float(data[5]))
        speed.append(float(data[6]))
length = len(speed)

t = np.arange(length)           # resampledTime

colalpha=[]
for i in range(0,len(speed)-1):
    if (alphaf[i]!=0):
        if speed[i]==1:
            colalpha.append('royalblue') 
        elif speed[i]==2:
            colalpha.append('lime')
        elif speed[i]==3:
            colalpha.append('r') 
    else:
        colalpha.append('k') 

coltheta=[]
for i in range(0,len(speed)-1):
    if (thetaf[i]!=0):
        if speed[i]==1:
            coltheta.append('royalblue') 
        elif speed[i]==2:
            coltheta.append('lime')
        elif speed[i]==3:
            coltheta.append('r') 
    else:
        coltheta.append('k') 

colz = []
for i in range(0,len(speed)-1):
    colz.append('k')   


def make_proxy(zvalue, scalar_mappable, **kwargs):
    color = scalar_mappable.cmap(scalar_mappable.norm(zvalue))
    return Line2D([0, 1], [0, 1], color=color, **kwargs)



fig, (ax1, ax2,ax3, ax4, ax5, ax6) = plt.subplots(6, 1, constrained_layout=True)

colors = ['black', 'royalblue', 'lime','red']
colorlines = [Line2D([0], [0], color=c, linewidth=1, linestyle='-') for c in colors]

ax1.plot(t,alpharaw)
ax1.set_title("Raw Pitch Angle (\u03F4)")
ax1.set_xlabel("Time Unit")
ax1.set_ylabel("\u03F4 (\N{DEGREE SIGN})")
ax1.set_ylim([-90,90])

lines = [((t0,alphaf0), (t1,alphaf1),(t2,alphaf2),(t3,alphaf3)) for t0, alphaf0, t1, alphaf1, t2,alphaf2, t3,alphaf3 in zip(t[1:], alphaf[1:], t[1:], alphaf[1:],t[:-1], alphaf[:-1], t[:-1], alphaf[:-1])]
colored_lines = LineCollection(lines, colors=colalpha, linewidths=(2,))
ax2.add_collection(colored_lines)
ax2.autoscale_view()
ax2.set_title("Filtered & Scaled & Selected Pitch Angle (\u03F4)")
ax2.legend(colorlines,['no speed','speed=1', 'speed=2','speed=3'],prop={"size":7})
ax2.set_xlabel("Time Unit")
ax2.set_ylabel("\u03F4")

ax3.plot(t,thetaraw)
ax3.set_title("Raw Roll Angle (\u03A6)")
ax3.set_xlabel("Time Unit")
ax3.set_ylabel("\u03A6 (\N{DEGREE SIGN})")
ax3.set_ylim([-90,90])

lines = [((t0,thetaf0), (t1,thetaf1),(t2,thetaf2),(t3,thetaf3)) for t0, thetaf0, t1, thetaf1, t2,thetaf2, t3,thetaf3 in zip(t[1:], thetaf[1:], t[1:], thetaf[1:],t[:-1], thetaf[:-1], t[:-1], thetaf[:-1])]
colored_lines = LineCollection(lines, colors=coltheta, linewidths=(2,))
ax4.add_collection(colored_lines)
ax4.autoscale_view()
ax4.set_title("Filtered & Scaled & Selected Roll Angle (\u03A6)")
ax4.legend(colorlines,['no speed','speed=1', 'speed=2','speed=3'],prop={"size":7})
ax4.set_xlabel("Time Unit")
ax4.set_ylabel("\u03A6")

ax5.plot(t,zraw)
ax5.set_title("Raw Z-axis Acceleration (a)")
ax5.set_xlabel("Time Unit")
ax5.set_ylabel("a (m/s\u00B2)")


lines = [((t0,zf0), (t1,zf1),(t2,zf2),(t3,zf3)) for t0, zf0, t1,zf1, t2,zf2, t3,zf3 in zip(t[1:], zf[1:], t[1:], zf[1:],t[:-1], zf[:-1], t[:-1], zf[:-1])]
colored_lines = LineCollection(lines, colors=colz, linewidths=(2,))
ax6.add_collection(colored_lines)
ax6.autoscale_view()
ax6.set_title("Filtered Z-axis Acceleration")
ax6.set_xlabel("Time Unit")
ax6.set_ylabel("a (m/s\u00B2)")

plt.show()



