import matplotlib.pyplot as plt
import networkx as nx

file = "C:/Users/andrl/Desktop/masterStuff/MasterData/Graph1.txt"
opened  = open(file, 'r')
d = {}
for line in opened:
   t = (line.split(','))
   t.remove('\n')
   d[t[0]] = t[1: len(t)]



G=nx.Graph()

for i in d:
    for j in d[i]:
        G.add_edge(i, j)


elarge=[(u,v) for (u,v,d) in G.edges(data=True)]

pos=nx.spring_layout(G) # positions for all nodes

# nodes
nx.draw_networkx_nodes(G,pos,node_size=100)

# edges
nx.draw_networkx_edges(G,pos,edgelist=elarge,
                    width=1)

# labels
#nx.draw_networkx_labels(G,pos,font_size=20,font_family='sans-serif')

plt.axis('off')
plt.savefig("graph.png") # save as png
plt.show() # display
