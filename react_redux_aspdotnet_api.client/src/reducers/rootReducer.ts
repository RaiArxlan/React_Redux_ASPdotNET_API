import { combineReducers } from 'redux';
import CounterReducer from './CounterReducer';

const rootReducer = combineReducers({
    counter: CounterReducer,
    // Add other reducers here
});

export default rootReducer;
