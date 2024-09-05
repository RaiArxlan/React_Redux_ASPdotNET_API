import { createSlice } from '@reduxjs/toolkit';

interface CounterState {
    counter: number;
}

const initialState: CounterState = {
    counter: 0,
};

const CounterReducer = createSlice({
    name: 'counter',
    initialState,
    reducers: {
        increase(state) {
            state.counter++;
        },
        decrease(state) {
            state.counter--;
        },
        reset(state) {
            state.counter = 0;
        }
    },
});

export const { increase, decrease, reset } = CounterReducer.actions;
export default CounterReducer.reducer;
