import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from tensorflow.keras.layers import Dense, Dropout, Embedding, LSTM
from tensorflow.keras.models import Sequential
from tensorflow.keras.preprocessing.sequence import pad_sequences
from tensorflow.keras.preprocessing.text import Tokenizer
from sklearn.metrics import accuracy_score, precision_score, recall_score, f1_score, classification_report

train = pd.read_csv('datasets/fake-news/train.csv')
test = pd.read_csv('datasets/fake-news/test.csv')
train_data = train.copy()
test_data = test.copy()

train_data = train_data.set_index('id', drop = True)

print('train data shape', train_data.shape)
print('train data head\n', train_data.head())

print('test data shape', test_data.shape)
print('test data head\n', test_data.head())

print('missing values counts\n', train_data.isnull().sum())

# dropping missing values from text columns alone.
train_data[['title', 'author']] = train_data[['title', 'author']].fillna(value = 'Missing')
train_data = train_data.dropna()
print('missing values counts n', train_data.isnull().sum())

length = []
[length.append(len(str(text))) for text in train_data['text']]
train_data['length'] = length
print('train data length\n', train_data.head())

print('min data length', min(train_data['length']), ', max data length', max(train_data['length']), ', average data length', round(sum(train_data['length'])/len(train_data['length'])))

print('count of less then 50 character', len(train_data[train_data['length'] < 50]))

# dropping the outliers
train_data = train_data.drop(train_data['text'][train_data['length'] < 50].index, axis = 0)
print('min data length', min(train_data['length']), ', max data length', max(train_data['length']), ', average data length', round(sum(train_data['length'])/len(train_data['length'])))

max_features = 4500

# Tokenizing the text - converting the words, letters into counts or numbers.
# We dont need to explicitly remove the punctuations. we have an inbuilt option in Tokenizer for this purpose
tokenizer = Tokenizer(num_words = max_features, filters='!"#$%&()*+,-./:;<=>?@[\\]^_`{|}~\t\n', lower = True, split = ' ')
tokenizer.fit_on_texts(texts = train_data['text'])
X = tokenizer.texts_to_sequences(texts = train_data['text'])

# now applying padding to make them even shaped.
X = pad_sequences(sequences = X, maxlen = max_features, padding = 'pre')

print('X shape', X.shape)
y = train_data['label'].values
print('Y shape', y.shape)

# splitting the data training data for training and validation.
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.2, random_state = 101)

#TODO delete
#X_train = X_train[:128, :]
#y_train = y_train[:128]

print('X train shape', X_train.shape)

# LSTM Neural Network
lstm_model = Sequential(name = 'lstm_nn_model')
lstm_model.add(layer = Embedding(input_dim = max_features, output_dim = 120, name = '1st_layer'))
lstm_model.add(layer = LSTM(units = 120, dropout = 0.2, recurrent_dropout = 0.2, name = '2nd_layer'))
lstm_model.add(layer = Dropout(rate = 0.5, name = '3rd_layer'))
lstm_model.add(layer = Dense(units = 120,  activation = 'relu', name = '4th_layer'))
lstm_model.add(layer = Dropout(rate = 0.5, name = '5th_layer'))
lstm_model.add(layer = Dense(units = len(set(y)),  activation = 'sigmoid', name = 'output_layer'))
# compiling the model
lstm_model.compile(optimizer = 'adam', loss = 'sparse_categorical_crossentropy', metrics = ['accuracy'])

lstm_model_fit = lstm_model.fit(X_train, y_train, epochs = 1)

test_data = test.copy()
print('test_data shape', test_data.shape)

test_data = test_data.set_index('id', drop = True)
print('test_data shape', test_data.shape)

test_data = test_data.fillna(' ')
print('test_data shape', test_data.shape)
print(test_data.isnull().sum())

#TODO delete
#test_data = test_data.iloc[:10, :]

tokenizer.fit_on_texts(texts = test_data['text'])

test_text = tokenizer.texts_to_sequences(texts = test_data['text'])

test_text = pad_sequences(sequences = test_text, maxlen = max_features, padding = 'pre')

lstm_prediction = lstm_model.predict(test_text)

lstm_prediction_vec = np.argmax(lstm_prediction, axis=1)

print("lstm_prediction", lstm_prediction_vec)

y_test_subset = y_test[:len(lstm_prediction_vec)];
lstm_prediction_vec_subset = lstm_prediction_vec[:len(y_test)];

accuracy = accuracy_score(y_test_subset, lstm_prediction_vec_subset)
precision = precision_score(y_test_subset, lstm_prediction_vec_subset, average='weighted')
recall = recall_score(y_test_subset, lstm_prediction_vec_subset, average='weighted')
f1 = f1_score(y_test_subset, lstm_prediction_vec_subset, average='weighted')

classification_rep = classification_report(y_test_subset, lstm_prediction_vec_subset)

print(f"Accuracy: {accuracy:.2f}")
print(f"Precision: {precision:.2f}")
print(f"Recall: {recall:.2f}")
print(f"F1-Score: {f1:.2f}")
print("\nClassification Report:\n", classification_rep)