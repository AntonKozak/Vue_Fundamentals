import api from './api.js'

export default {
  async getMenuItems() {
    try {
      const response = await api.get('/menuItem')

      if (response.data.isSuccess) {
        return response.data.result
      } else {
        console.log('Menu items fetched:', response.data)
        throw new Error('Failed to fetch menu items')
      }
    } catch (error) {
      console.error('Error fetching menu items:', error)
      throw error
    }
  },
  async getMenuItemById(id) {
    try {
      const response = await api.get(`/menuItem/${id}`)
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        console.log('Menu item fetched:', response.data)
        throw new Error('Failed to fetch menu item')
      }
    } catch (error) {
      console.error('Error fetching menu item:', error)
      throw error
    }
  },
  async createMenuItem(formData) {
    try {
      const response = await api.post('/menuItem', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        console.log('Menu item created:', response.data)
        throw new Error('Failed to create menu item')
      }
    } catch (error) {
      console.error('Error creating menu item:', error)
      throw error
    }
  },
  async updateMenuItem(id, formData) {
    try {
      const response = await api.put(`/menuItem/${id}`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        console.log('Menu item updated:', response.data)
        throw new Error('Failed to update menu item')
      }
    } catch (error) {
      console.error('Error updating menu item:', error)
      throw error
    }
  },
  async deleteMenuItem(id) {
    try {
      const response = await api.delete(`/menuItem/${id}`)
      if (response.data.isSuccess) {
        return response.data.result
      } else {
        console.log('Menu item deleted:', response.data)
        throw new Error('Failed to delete menu item')
      }
    } catch (error) {
      console.error('Error deleting menu item:', error)
      throw error
    }
  },
}
