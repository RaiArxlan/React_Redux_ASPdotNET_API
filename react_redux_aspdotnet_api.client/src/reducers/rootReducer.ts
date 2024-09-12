import { combineReducers } from 'redux';
import CounterReducer from './CounterReducer';
import UserReducer from './UserReducer';

const rootReducer = combineReducers({
    counter: CounterReducer,
    user: UserReducer,
    // Add other reducers here

});

export default rootReducer;
