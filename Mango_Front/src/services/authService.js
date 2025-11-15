


  import api from './api.js';

export default {

    async signUp(userData) {
        try {
            const response = await api.post('/auth/register', {
                  email: userData.email,
                  name: userData.name,
  password: userData.password,
  confirmPassword: userData.confirmPassword,
  role: userData.role
            });

            console.log('Sign Up response:', response.data);

            if (response.data.isSuccess) {
                return { success: true, message: 'User registered successfully' };
            }
            else {
                throw new Error('Failed to create menu item');
            }
        } catch (error) {
            console.error('Error register:', error);
            throw error;
        }
    },

     async signIn(userData) {
    try {
      const response = await api.post('/auth/login', {
        email: userData.email,
        password: userData.password,
      })

      if (response.data.isSuccess) {
        const { token, email } = response.data.result
        const payload = JSON.parse(atob(token.split('.')[1]))
        return {
          token,
          user: {
            email,
            role: payload.role,
            name: payload.fullname,
            id: payload.id,
          },
        }
      } else {
        throw new Error('Login failed')
      }
    } catch (error) {
      console.error('Error in Login:', error)
      throw error
    }
  },
}
