#!/usr/bin/env python
# coding: utf-8

# In[1]:


import pandas as pd
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score
import numpy as np


# In[2]:


X = pd.read_csv('ECG_dataX.csv')
Y = pd.read_csv('ECG_dataY.csv')

# Splitting the data
X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.2, random_state=0)

# Verifying the shapes of the splits
X_train.shape, Y_train.shape, X_test.shape, Y_test.shape


# In[3]:


X_train = np.ascontiguousarray(X_train.to_numpy().astype('float32'))
X_test = np.ascontiguousarray(X_test.to_numpy().astype('float32'))
Y_train = Y_train.values.ravel()
Y_test = Y_test.values.ravel()

param_grid_knn = {'n_neighbors': list(range(1, 31))}
param_grid_dt = {'max_depth': list(range(1, 21))}
param_grid_rf = {'max_depth': list(range(1, 21))}


# In[4]:


# Initialize GridSearchCV for each classifier

grid_search_knn = GridSearchCV(KNeighborsClassifier(), param_grid_knn, cv=5,)
grid_search_dt = GridSearchCV(DecisionTreeClassifier(random_state=0), param_grid_dt, cv=5,)
grid_search_rf = GridSearchCV(RandomForestClassifier(random_state=0), param_grid_rf, cv=5,)


# In[5]:


# List of grid searches for ease of iteration
grid_searches = [('KNeighbors', grid_search_knn),
                 ('DecisionTree', grid_search_dt),
                 ('RandomForest', grid_search_rf)]


# In[6]:


# Prepare a dictionary to store accuracies and best parameters
results = {
    ' ': [],
    'Training Accuracy': [],
    'Test Accuracy': []
}


# In[7]:


# Perform grid search and evaluate each classifier
for name, grid_search in grid_searches:
    # Fit the grid search to the data
    grid_search.fit(X_train, Y_train)
    
    # Get the best estimator and parameters
    best_estimator = grid_search.best_estimator_
    best_param = grid_search.best_params_
    
    # Predict on training set and test set using the best estimator
    Y_train_pred = best_estimator.predict(X_train)
    Y_test_pred = best_estimator.predict(X_test)
    
    # Calculate accuracies
    train_accuracy = accuracy_score(Y_train, Y_train_pred)
    test_accuracy = accuracy_score(Y_test, Y_test_pred)
    print(f'{name} done')
    # Append the results to the dictionary
    results[' '].append(name)
   # results['Best Parameter'].append(best_param)
    results['Training Accuracy'].append(train_accuracy)
    results['Test Accuracy'].append(test_accuracy)


# #results_df = pd.DataFrame(results)
# #results_df
# for key, value in results.items():
#     print(f"Length of {key}: {len(value)}")
# 

# In[9]:


results_df = pd.DataFrame(results)

results_df_transposed = results_df.set_index(' ').T

# Display the DataFrame
results_df_transposed


# In[ ]:




