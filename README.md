# image-quantization

What is Image Quantization ?
The idea of color quantization is to reduce the number of colors in a full resolution digital color image (24 bits per pixel) to a smaller set of representative colors called color palette. Reduction should be performed so that the quantized image differs as little as possible from the original image. Algorithmic optimization task is to find such a color palette that the overall distortion is minimized.

Image Quantization Algorithm
To Apply the Clustering algorithm on the Image Quantization Problem, we need to
⦁	Find the distinct colors D = {d1, d2, d3 ….dm} from the input image. Can be known from the image histogram.
⦁	Construct a fully-connected undirected weighted graph G with
⦁	D vertices (number of distinct colors). 
⦁	Each pair of vertices is connected by a single edge. 
⦁	Edge weight is set as the Euclidean Distance between the RGB values of the 2 vertices.
⦁	Construct  a :
⦁	minimum-spanning-tree algorithm (a greedy algorithm in graph theory)
⦁	Input: connected undirected weighted graph
⦁	Output: a tree that keeps the graph connected with minimum total cost
⦁	Methodology: treats the graph as a forest and each node is initially represented as a tree. A tree is connected to another only and only if it has the least cost among all available. 
⦁	Extract the desired number of clusters (K) with maximum distances between each other. 
⦁	Find the representative color of each cluster.
⦁	Quantize the image by replacing the colors of each cluster by its representative color.
