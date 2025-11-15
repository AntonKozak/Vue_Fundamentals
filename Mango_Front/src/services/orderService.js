import api from './api.js'

export default {
  async getOrders(userId = null) {
    try {
      const response = await api.get('/Order', {
        params: {
          userId: userId,
        },
      })

      if (response.data.isSuccess) {
        return response.data.result
      } else {
        throw new Error('Failed to fetch order')
      }
    } catch (error) {
      console.error('Error fetching Orders:', error)
      throw error
    }
  },

  async getOrderById(orderId) {
    try {
      const response = await api.get(`/Order/${orderId}`)
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        throw new Error('Failed to fetch order')
      }
    } catch (error) {
      console.error('Error fetching order details:', error.message)
      throw error
    }
  },

  async createOrder(orderData) {
    try {
      const response = await api.post('/Order', {
        pickUpName: orderData.pickUpName,
        pickUpPhoneNumber: orderData.pickUpPhoneNumber,
        pickUpEmail: orderData.pickUpEmail,
        applicationUserId: orderData.applicationUserId,
        orderTotal: orderData.orderTotal,
        totalItem: orderData.totalItem,
        orderDetails: orderData.orderDetails,
      })
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        throw new Error('Failed to place order')
      }
    } catch (error) {
      console.error('Error placing order:', error.message)
      throw error
    }
  },

  async updateOrder(orderId, orderData) {
    try {
      const response = await api.put(`/Order/${orderId}`, {
        orderHeaderId: orderId,
        status: orderData.status,
      })

      if (response.data.isSuccess) {
        return response.data.result
      } else {
        throw new Error('Failed to update order')
      }
    } catch (error) {
      console.error('Error updating order:', error.message)
      throw error
    }
  },
}
